#include <cstdio>
#include <cstring>
#include <cstdlib>
#include <unistd.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <sys/types.h>
#include <sys/epoll.h>
#include <errno.h>
#include <string>
#include <list>
#include <iostream>
#include <map>
#include <fcntl.h>

#include "protobuf_test.pb.h"

#define MAXEVENTS 64

struct Server{
    int port;
    int listen_fd;
    int epoll_fd;
    int headLength;
    int nowHeadLength;
    int bodyLength;
    int nowBodyLength;
    struct epoll_event event;
    struct epoll_event* events;
    FILE *fp1, *fp2, *fp3, *fp4;
    std::list<int> clientlist;
    std::map<int,std::string> socketlist;
    std::string str;
    LogMsg logMsg;
    void Init(){
        this->port = 3360;
        (this->clientlist).clear();
        this->headLength = 4;
        this->nowHeadLength = 0;
        this->bodyLength = 0;
        this->nowBodyLength = 0;
        this->events = (struct epoll_event*) malloc (sizeof(event)*MAXEVENTS);
    }
    void Release(){
        if((this->events) != NULL) free(this->events);
        if((this->fp1) != NULL) free(this->fp1);
        if((this->fp2) != NULL) free(this->fp2);
        if((this->fp3) != NULL) free(this->fp3);
        if((this->fp4) != NULL) free(this->fp4);
    }
    void SetPort(int _port){
        this->port = _port;
    }
    bool SetServerSocket(){
        listen_fd = socket(AF_INET, SOCK_STREAM, 0);
        printf("listen: %d\n", listen_fd);
        if(listen_fd == -1){
            printf("create server socket error, errno = %d, (%s)\n", errno, strerror(errno));
            return false;
        }

        // address
        struct sockaddr_in listen_addr;
        listen_addr.sin_family = AF_INET;
        listen_addr.sin_addr.s_addr = htonl(INADDR_ANY);
        listen_addr.sin_port = htons(port);

        // bind
        int bind_ret = bind(listen_fd, (struct sockaddr*)&listen_addr, sizeof(listen_addr));
        if(bind_ret == -1){
            printf("bind error, errno = %d, (%s)\n", errno, strerror(errno));
            return false;
        } else {
            printf("bind ret: %d\n", bind_ret);
        }

        // listen
        int listen_ret = listen(listen_fd, 128);
        if(listen_ret == -1){
            printf("listen error, errno = %d (%s)\n", errno, strerror(errno));
            return false;
        } else {
            printf("listen ret: %d\n", listen_ret);
        }

        // epoll
        epoll_fd = epoll_create(10);
        if(epoll_fd == -1){
            printf("epoll create error, errno = %d (%s)\n", errno, strerror(errno));
            return false;
        }
        event.data.fd = listen_fd;
        event.events = EPOLLIN;
        
        int ret = epoll_ctl(epoll_fd, EPOLL_CTL_ADD, listen_fd, &event);
        if(ret == -1){
            printf("epoll ctl error, errno = %d (%s)\n", errno, strerror(errno));
            return false;
        }
        return true;
    }
    bool GetDataStream(int fd, char *data){
        bodyLength = BytesToInt(data);
        //printf("%d\n", bodyLength);
        nowBodyLength = 0;
        char *buff = (char*) malloc (sizeof(char)*bodyLength);
        while(nowBodyLength < bodyLength){
            int ret = recv(fd, buff + nowBodyLength, bodyLength - nowBodyLength, 0);
            if(ret == -1){
                printf("recv error, errno = %d (%s)\n", errno, strerror(errno));
                return false;
            }
            nowBodyLength += ret;
        }
        str = "";
        for(int i = 0; i < bodyLength; i++ ) str += buff[i];
        free(buff);
        logMsg.ParseFromString(str);
        //std::cout << logMsg.optype() << '\n' << logMsg.username() << '\n' << logMsg.userpwd() << '\n';
        return true;
    }
    void InitPath(char **logpath, char **pospath, char **tagpath){
        std::string a = "/root/user_list/";
        std::string b = "/root/user_pos/";
        std::string c = "/root/user_login/";
        std::string d = logMsg.username();
        std::string e = ".txt";

        std::string logpath_str = a + d + e;
        std::string pospath_str = b + d + e;
        std::string tagpath_str = c + d + e;

        *logpath = (char*)logpath_str.c_str();
        *pospath = (char*)pospath_str.c_str();
        *tagpath = (char*)tagpath_str.c_str();
    }
    std::string IntToString(int num){
        std::string ret = "";
        if(num < 0){
            ret += '-';
            num = -num;
        }
        if(num > 9) ret += IntToString(num / 10);
        ret += '0' + num % 10;
        return ret;
    }
    void WriteMsgToTxt(const char *str, FILE *fp){
        if(fp == NULL) return;
        fputs(str, fp);
        fclose(fp);
    }
    void SendMsgToClient(int fd, char *data, char flag){
        data[0] = flag;
        int ret = send(fd, data, strlen(data), 0);
        if(ret == -1){
            printf("send to socket: %d error, errno = %d (%s)\n",fd, errno, strerror(errno));
        }
    }
    bool CompareUserPassword(){
        char s[30];
        fgets(s, 50, fp1);
        int cmpret = strcmp(s, (char*)logMsg.userpwd().c_str());
        return cmpret == 0;
    }
    bool GetUserLoginState(char *tagpath){
        fp2 = fopen(tagpath, "r");
        char tag[5];
        fgets(tag, 10, fp2);
        fclose(fp2);
        return strcmp(tag, "0") == 0;
    }
    void PositionInfo(char *pospath){
        fp3 = fopen(pospath, "r");
        for(int i = 0; i < 3; i++ ){
            char t[30];
            fgets(t, 50, fp3);
            t[strlen(t)-1] = '\0';
            double posnum = strtod(t, NULL);
            printf("%.f\n",posnum);
        }
        fclose(fp3);
    }
    void ListNowClientlist(){
        std::list<int>::iterator it = clientlist.begin();
        while(it != clientlist.end()){
            printf("%d ",*it);
            it++;
        }
        printf("\n");
    }
    void AddClientUser(int fd){
        clientlist.push_back(fd);
    }
    void CreateSocket(int fd){
        socketlist[fd] = logMsg.username();
    }
    void UserLogin(int fd, char *data, char *tagpath, char *pospath){
        if(fp1 == NULL){ // user not found
            SendMsgToClient(fd, data, '1');
        } else { // user exist
            // compare user's password
            if(CompareUserPassword()){ // password correct
                // get user's login state
                if(GetUserLoginState(tagpath)){ // login success
                    SendMsgToClient(fd, data, '2');
                    // change login mode
                    WriteMsgToTxt("1", fopen(tagpath, "w"));
                    // position info
                    PositionInfo(pospath);
                    // list now clientlist
                    ListNowClientlist();
                    // add now client to clientlist
                    AddClientUser(fd);
                    // create socket and add username to map
                    CreateSocket(fd);
                } else { // user already login
                    SendMsgToClient(fd, data, '3');
                }
            } else { // password wrong
                SendMsgToClient(fd, data, '4');
            }
        }
    }
    void CreateUserMsg(int fd, char *data, char *logpath){
        fp2 = fopen(logpath, "a+");
        fputs((char*)logMsg.userpwd().c_str(), fp2);
        SendMsgToClient(fd, data, '2');
        fclose(fp2);
    }
    void CreatePosMsg(int fd, char *data, char *pospath){
        fp3 = fopen(pospath, "a+");
        for(int i = 0; i < 3; i++ ){
            fputs("0\n", fp3);
        }
        fclose(fp3);
    }
    void CreateLoginMsg(int fd, char *data, char *tagpath){
        fp4 = fopen(tagpath, "a+");
        fputs("0", fp4);
        fclose(fp4);
    }
    void UserRegister(int fd, char *data, char *logpath, char *pospath, char *tagpath){
        //printf("!!!\n");
        if(fp1 != NULL){ // user already exist
            SendMsgToClient(fd, data, '1');
        } else { // can register
            // create login_msg_txt to user_list
            CreateUserMsg(fd, data, logpath);
            // create pos_txt to user_pos
            CreatePosMsg(fd, data, pospath);
            // create login_state_txt to user_login
            CreateLoginMsg(fd, data, tagpath);
        }
    }
    bool CloseSocket(int fd){
        printf("socket %d closed\n", fd);
        // delete socket_fd;
        clientlist.remove(fd);
        // get username
        std::map<int,std::string>::iterator it = socketlist.find(fd);
        if(it != socketlist.end()){
            WriteMsgToTxt("0", fopen((char*)("/root/user_login/" + socketlist[fd] + ".txt").c_str(), "w"));
            socketlist.erase(it);
        }
        int ret = close(fd);
        if(ret == -1){
            printf("close socket error, errno = %d (%s)\n", errno, strerror(errno));
            return false;
        }
        return true;
    }
    int BytesToInt(char *src){
        int ret = 0;
        ret |= (int)(src[0] & 0xff);
        ret |= (int)((src[1] & 0xff) << 8);
        ret |= (int)((src[2] & 0xff) << 16);
        ret |= (int)((src[3] & 0xff) << 24);
        return ret;
    }
    void Work(){
        while(true){
            //printf("%d\n",clientlist.size());
            int n = epoll_wait(epoll_fd, events, MAXEVENTS, -1);
            if(n == -1){
                printf("epoll wait error, errno = %d (%s)\n", errno, strerror(errno));
                break;
            }
            for(int i = 0; i < n; i++ ){
                int fd = events[i].data.fd;
                int fd_events = events[i].events;
                if((fd_events & EPOLLERR) ||
                   (fd_events & EPOLLHUP) ||
                   (!(fd_events & EPOLLIN))) {
                    printf("fd: %d error\n", fd);
                    close(fd);
                    continue;
                } else if (fd == listen_fd){
                    struct sockaddr client_addr;
                    socklen_t client_addr_len = sizeof(client_addr);
                    int new_fd = accept(listen_fd, &client_addr, &client_addr_len);
                    if(new_fd == -1){
                        printf("accept socket error, errno = %d (%s)\n", errno, strerror(errno));
                        continue;
                    }
                    printf("new socket: %d\n", new_fd);
                    
                    // set nonblock
                    int flags = fcntl(new_fd, F_GETFL, 0);
                    fcntl(new_fd, F_SETFL, flags | O_NONBLOCK);
                    
                    event.data.fd = new_fd;
                    event.events = EPOLLIN;
                    int ret = epoll_ctl(epoll_fd, EPOLL_CTL_ADD, new_fd, &event);
                    if(ret == -1){
                        printf("epoll ctl error, errno = %d (%s)\n", errno, strerror(errno));
                        continue;
                    }
                } else {
                    char data[1024] = {0};
                    int ret = recv(fd, data + nowHeadLength, headLength - nowHeadLength, 0);
                    printf("%d\n", ret);
                    if(ret > 0){
                        //printf("%d\n", ret);
                        nowHeadLength += ret;
                        if(nowHeadLength < headLength) continue;
                        nowHeadLength = 0;
                        // get data stream
                        GetDataStream(fd, data);

                        // init path
                        char *logpath, *pospath, *tagpath;
                        InitPath(&logpath, &pospath, &tagpath);
                        
                        // user operation
                        memset(data, 0, 1024);
                        fp1 = fopen(logpath, "r");
                        if(logMsg.optype() == 0){ // login
                            UserLogin(fd, data, tagpath, pospath);
                        } else if(logMsg.optype() == 1){ // register
                            UserRegister(fd, data, logpath, pospath, tagpath);
                        }
                        if(fp1 != NULL) fclose(fp1);
                    }
                    if(ret == 0){
                        // close socket
                        //printf("!!!\n");
                        if(!CloseSocket(fd)) continue;
                    } else if (ret == -1){
                        printf("recv error, errno = %d (%s)\n", errno, strerror(errno));
                    }
                }
            }
        }
    }
};


int main(int argc, char** argv){
    Server server;
    server.Init();
    if (argc > 1) server.SetPort(atoi(argv[1]));
    if (!server.SetServerSocket()) return 0;
    server.Work();
    server.Release();
    return 0;
}

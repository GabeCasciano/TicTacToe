from socket import *

port = 8888
addr = "127.0.0.1"

sock = socket(AF_INET, SOCK_STREAM)
sock.connect((addr,port))

print("Connected")

sock.send(b'gabe')
temp = sock.recv(1024)
print(temp)

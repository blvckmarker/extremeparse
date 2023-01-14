import socket
import sys;


def start_server(envf: str):
    host = '127.0.0.1'
    port = 0

    server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server.bind((host, port))

    with open(envf, 'w') as fs:
        host, port = server.getsockname()
        fs.write(f"PORT={port}\nHOST={host}")

    server.listen()
    print(f'Listening on {host}:{port}')

    while input() != 's':
        try:
            conn, addr = server.accept()
        except:
            print('server accept timeout')
            return

        print(f'Connection from {conn}')
        data = conn.recv(1024)
        if not data:
            break

        server.sendall(data);
        conn.close()

    server.shutdown(socket.SHUT_RDWR)
    server.close()



if __name__ == "__main__":
    envfile = sys.argv[1]
    start_server(envfile)

import socket
import sys;


def start_server(envf: str):
    host = '127.0.0.1'
    port = 0


    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
        s.bind((host, port))
        with open(envf, 'w') as fs:
            host, port = s.getsockname()
            fs.write(f"PORT={port}\nHOST={host}")
        s.listen()
        
        print(f'Start listening on {port}')
        conn, addr = s.accept()
        while True:
            print(port)
            data = conn.recv(1024) 
            if not data:
                break

            print('[Server] Recieved: ' + data.decode())

            response = '{"Name": "Samuel", "Description": "hallo", "Info": "kekw"}'.encode() 
            conn.sendall(response)
        print('Exited')

if __name__ == "__main__":
    envfile = sys.argv[1]
    start_server(envfile)

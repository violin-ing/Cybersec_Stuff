import socket
import ipaddress
import sys

def port_scan(ip_range, ports):
	for ip in ip_range:
		print(f"Scanning {ip}")
		for port in ports:
			sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
			sock.settimeout(2)
			result = sock.connect_ex((str(ip), port))
			if result == 0:
				print(f"Port {port} is open on {ip}")
			sock.close()

ip_range = ipaddress.IPv4Network(sys.argv[1], strict=False)
ports = [80, 443, 8080]

port_scan(ip_range, ports)

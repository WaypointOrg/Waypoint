#!/bin/sh

interface="wlp110s0"

printf "[DEBUG:] Disbaling ufw...\n"
ufw disable

printf "[DEBUG:] Updating iptables...\n"
echo "1" > /proc/sys/net/ipv4/ip_forward
iptables --table nat --append POSTROUTING --out-interface $interface -j MASQUERADE
iptables --append FORWARD --in-interface hotspot -j ACCEPT
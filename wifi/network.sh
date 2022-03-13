#!/bin/sh

interface="wlp110s0"

# printf "[DEBUG:] Updating iptables...\n"
# echo "1" > /proc/sys/net/ipv4/ip_forward
# iptables --table nat --append POSTROUTING --out-interface $interface -j MASQUERADE
# iptables --append FORWARD --in-interface hotspot -j ACCEPT

printf "[DEBUG:] creating virtual interface...\n"
iw phy phy0 interface add hotspot type __ap

printf "[DEBUG:] starting virtual interface...\n"
ifconfig hotspot 10.0.0.1

printf "[DEBUG:] launching hostapd...\n"
hostapd network.conf 
# > hostapd_output.txt

printf "[DEBUG:] deleting virtual interface...\n"
iw dev hotspot del
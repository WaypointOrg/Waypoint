#!/bin/sh

# printf "[DEBUG:] copying dnsmasq.conf...\n"
# cp -f dnsmasq.conf /etc/dnsmasq.conf

printf "[DEBUG:] Starting dnsmasq...\n"
dnsmasq -d 

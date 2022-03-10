#!/bin/sh

start() {
    printf "[DEBUG:] Stopping systemd-resolved...\n"
    systemctl stop systemd-resolved
    sleep 0.5

    printf "[DEBUG:] copying dnsmasq.conf...\n"
    cp -f dnsmasq.conf /etc/dnsmasq.conf

    printf "[DEBUG:] Starting dnsmasq...\n"
    dnsmasq -d 
}

quit() {
    printf "\n"
    printf "[DEBUG:] Restarting systemd-resolved...\n"
    systemctl start systemd-resolved
    systemctl restart systemd-resolved
    
    printf "[DEBUG:] Quitting...\n"
    exit 1
}

trap quit INT
start
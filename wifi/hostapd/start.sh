#!/bin/bash

printf "[DEBUG:] creating virtual interface...\n"
iw phy phy0 interface add hotspot type __ap

printf "[DEBUG:] removing managment from nmcli...\n"
nmcli device set hotspot managed no

printf "[DEBUG:] starting virtual interface...\n"
ifconfig hotspot 10.0.0.1

printf "[DEBUG:] launching hostapd...\n"
hostapd network.conf

printf "[DEBUG:] deleting virtual interface...\n"
iw dev hotspot del
#!/bin/sh

tmux new-session \; \
  send-keys 'cd iptables && ./update_iptables.sh && cd ../hostapd && ./start.sh' C-m \; \
  split-window -h \; \
  send-keys 'sleep 2 && cd dnsmasq && ./start.sh' C-m \; \

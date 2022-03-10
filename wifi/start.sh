#!/bin/sh

tmux new-session \; \
  send-keys './network.sh' C-m \; \
  split-window -h \; \
  send-keys 'sleep 2 && ./dnsmasq.sh' C-m \;
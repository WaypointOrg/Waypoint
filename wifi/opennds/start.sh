#!/bin/sh

printf "[DEBUG:] copying opennds.conf...\n"
cp -f opennds.conf /etc/opennds/opennds.conf

printf "[DEBUG:] starting opennds...\n"
opennds
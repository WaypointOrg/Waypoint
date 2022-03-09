# Waypoint wifi

## Installation
### 1. Download or clone this repository
```
git clone https://github.com/DanielRoulin/waypoint.git
```
### 2. Install dependencies
Waypoint wifi uses hostapd to generate the wifi hotspot, dnsmasq as a dhcp server and tmux for debugging.
```
sudo apt install hostapd dnsmasq tmux
```

## Customizing Waypoint wifi
### 1. Find your interface name
When runnimg Waypoint wifi, you need to redirect traffic on the hotspot to your other connetion. In order to do that, you first need to find the name of your interface. 
Run this command to list all of your interfaces:
```
ifconfig -a
```
In the output, find the interface corresponding to your other connection. It should look something like wl* (wlan0, wlp3s0, ..) if you are using a wireless interface  or eth* if you are connected with an Ethernet cable.

### 2. Customize your network
Your hotspot configuration is located in hostapd/network.conf  
You can customize a number of parameters by modify this file.  
The most important one is probably the ```ssid``` one, the name of your hotspot.


## Running Waypoint wifi
### Option 1: tmux
If your using tmux, simply run:
```
sudo ./start.sh
```

#### WARNING:
Running tmux as root spawns a root shell.  
**BE VERY CAREFUL ABOUT WHAT YOU DO.**

## Sources
https://anooppoommen.medium.com/create-a-wifi-hotspot-on-linux-29349b9c582d  
https://trevphil.com/posts/captive-portal  
https://rachitpandya.medium.com/how-to-create-a-captive-portal-38aba6284b91
#!/bin/bash

# Launch script for running AnypocApp on IMX93 with framebuffer

export AVALONIA_USE_FRAMEBUFFER=1

# Set the framebuffer device (usually /dev/fb0)
export FBDEV=/dev/fb0

# Run the application
./AnypocApp --framebuffer

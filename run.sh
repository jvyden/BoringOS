#!/bin/bash

# Sourced from https://cosmosos.github.io/articles/Installation/Running.html

helpFunction()
{
   printf "\n"
   printf "Usage: %s -i <ISO> -m <memory size>\n" "$0"
   printf "\t-i ISO path to be used\n"
   printf "\t-m Memory size to allocate to the Virtual Machine\n"
#   printf "\t-h Hard disk image location, can be created with qemu-img\n"
   exit 1 # Exit script after printing help
}

while getopts "i:m:" opt
do
   case "$opt" in
      i ) ISO="$OPTARG" ;;
      m ) MEMORY_SIZE="$OPTARG" ;;
#      h ) HDD_IMAGE="$OPTARG" ;;
      ? ) helpFunction ;; # Print helpFunction in case parameter is non-existent
   esac
done

# Print helpFunction in case parameters are empty
if [ -z "$ISO" ] || [ -z "$MEMORY_SIZE" ]
then
   echo "Some or all of the parameters are empty";
   helpFunction
fi

# Emulate the ISO
qemu-system-x86_64 -boot d -cdrom "$ISO" -m "$MEMORY_SIZE" \
  -enable-kvm -cpu host \
  -netdev tap,id=net0,ifname=tap0,script=no,downscript=no -device rtl8139,netdev=net0 \
  -vga qxl

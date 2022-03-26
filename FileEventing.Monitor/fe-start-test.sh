#!/bin/bash

count=1
rm -f /tmp/test-file.txt
touch /tmp/test-file.txt
while /bin/true; do
    echo $count
    echo $count >> /tmp/test-file.txt
    count=`expr $count + 1`
    sleep 1
done


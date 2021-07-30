#! /usr/bin/bash

n=1

while (( $n <= 20 ))
do
    if (( $n%3 == 0 ))
    then
        printf "Fizz"
    fi
    if (( $n%5 == 0 ))
    then
        printf "Buzz"
    fi
    if (( $n%5 != 0 && n%3 != 0 ))
    then
        printf "$n"
    fi
    printf "\n"
    n=$(( n+1 ))
done
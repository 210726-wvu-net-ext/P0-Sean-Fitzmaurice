#! /usr/bin/bash

read -p "Enter a Number: " num

if (( num%2 == 0 ))
then
    echo "That is an even number"
else
    echo "That is an odd number"
fi

read -p "Enter marks: " mark

if (( mark > 100 || mark < 0 ))
then
    echo "That is an invalid mark"
elif (( mark <= 40 ))
then
    echo "You got an F"
elif (( mark <= 50 ))
then
    echo "You got a D"
elif (( mark <= 60))
then
    echo "You got a C"
elif (( mark <= 70 ))
then
    echo "You got a B"
else
    echo "You got an A"
fi


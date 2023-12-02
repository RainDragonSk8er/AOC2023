#!/bin/bash
for i in {1..25..1}; do 
    for name in "jonathan" "theodor" "kristian" "hallvard"; do
        mkdir -p Des${i}/${name}
        touch Des${i}/${name}/.hiddenfile
    done
done
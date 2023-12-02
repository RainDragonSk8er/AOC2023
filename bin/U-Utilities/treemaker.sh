#!/bin/bash
for i in {1..25..1}; do 
    for name in "jonathan" "theodor" "kristian" "hallvard"; do
        printf -v j "%02d" $i
        path="Des-${j}/${name}"
        mkdir -p ${path}
        touch ${path}/.hiddenfile
    done
done
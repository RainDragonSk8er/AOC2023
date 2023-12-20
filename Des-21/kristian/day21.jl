using DataStructures
using Memoization
using AStarSearch

fn = "input.txt"
fn = "test1.txt"
lines = readlines(fn)
M = reduce(vcat, permutedims.(collect.(lines)))
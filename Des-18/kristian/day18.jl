using Memoization
using DataStructures
using AStarSearch

fn = "input.txt"
fn = "test1.txt"

global debug = true
lines = readlines(fn)
#M = reduce(vcat, permutedims.(collect.(lines)))
directions = Dict("U" => [0, 1], "R" => [1, 0], "D" => [0, -1], "L" => [-1, 0])
start = [[1, 1], ""]
points = [start]
circ = 0
for (i, line) in enumerate(lines)
    # debug ? println("i:$i") : nothing
    parts = split(line)
    direction = directions[parts[1]]
    steplength = parse(Int, parts[2])
    circ += steplength
    color = parts[3][2:end-1]
    coords = points[end][1] + direction*(steplength)
    push!(points, [coords, color])
end
area = 0
for i in 2:length(points)
    curr = points[i][1]
    prev = points[i-1][1]
    area += prev[1]*curr[2] - curr[1]*prev[2]
end
area
area = abs(area) + circ + 2
# first = points[1][1]
# last = points[end][1]
# area += last[1]*first[2] - first[1]*last[2]
area /= 2

#part 2======================================================
fn = "input.txt"
fn = "test1.txt"

global debug = true
lines = readlines(fn)
#M = reduce(vcat, permutedims.(collect.(lines)))
directions = Dict("U" => [0, 1], "R" => [1, 0], "D" => [0, -1], "L" => [-1, 0])
numberToDir = Dict('0' => "R", '1' => "D", '2' => "L", '3' => "U")
start = [[1, 1], ""]
points = [start]
circ = 0
for (i, line) in enumerate(lines)
    # debug ? println("i:$i") : nothing
    parts = split(line)
    color = parts[3][2:end-1]
    direction = directions[numberToDir[color[end]]]
    steplength = parse(Int, color[2:6], base=16)
    
    circ += steplength
    coords = points[end][1] + direction*(steplength)
    push!(points, [coords, color])
end
area = 0
for i in 2:length(points)
    curr = points[i][1]
    prev = points[i-1][1]
    area += prev[1]*curr[2] - curr[1]*prev[2]
end
area
area = abs(area) + circ + 2
# first = points[1][1]
# last = points[end][1]
# area += last[1]*first[2] - first[1]*last[2]
area /= 2


#Visualization==================================
using Plots
plot(axis=false, xticks=nothing, yticks=nothing)
for i in 2:length(points)
    curr = points[i][1]
    prev = points[i-1][1]
    plot!([prev[1], curr[1] ], [prev[2], curr[2]], legend=false, color=:black)
end
display(current())
savefig("part1path.pdf")
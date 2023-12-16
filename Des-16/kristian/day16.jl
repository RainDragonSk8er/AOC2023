using Memoization
function Move(M, currentpos::CartesianIndex, previouspos::CartesianIndex, visited, debug)
    debug ? println("Move, current:$currentpos, previouspos:$previouspos") : nothing
    currentdirection = currentpos - previouspos
    Msize = size(M)
    if (currentpos, previouspos) in visited
        debug ? println("hit an earlier beam: $currentpos, $previouspos") : nothing
        return visited
    end
    if !all( (1,1) .<= Tuple(currentpos) .<= Msize )
        debug ? println("$currentpos out of bounds $Msize") : nothing
        return visited
    end
    push!(visited, (currentpos, previouspos))

    if currentdirection in [ CartesianIndex((0,1)), CartesianIndex((0,-1))]
        continuechar = '-'
    elseif currentdirection in [ CartesianIndex((1,0)), CartesianIndex((-1,0))]
        continuechar = '|'
    end
    while all( (0,0) .<= Tuple(currentpos + currentdirection) .<= (Msize[1] + 1, Msize[2]+1) ) && M[currentpos] in ['.', continuechar] 
        currentpos += currentdirection
        push!(visited, (currentpos, previouspos))
    end
    if !all( (1,1) .<= Tuple(currentpos) .<= Msize )
        debug ? println("beam leaving $currentpos") : nothing
        pop!(visited, (currentpos, previouspos))
        return visited
    end
    debug ? println("straight line to $currentpos") : nothing
    letter = M[currentpos]
    
    if letter == '|'
        @assert Tuple(currentpos - previouspos)[1] == 0
        visited = Move(M, currentpos + CartesianIndex((1,0)), currentpos, visited, debug) 
        visited = Move(M, currentpos + CartesianIndex((-1,0)), currentpos, visited, debug)  
    elseif letter == '-'
        @assert Tuple(currentpos - previouspos)[2] == 0
        visited = Move(M, currentpos + CartesianIndex((0,1)), currentpos, visited, debug) 
        visited = Move(M, currentpos + CartesianIndex((0,-1)), currentpos, visited, debug)
    elseif letter == '/'
        if currentdirection == CartesianIndex((1,0))
            step  = (0, -1)
        elseif currentdirection == CartesianIndex((0,-1))
            step = (1,0)
        elseif currentdirection == CartesianIndex((-1,0))
            step = (0,1)
        elseif currentdirection == CartesianIndex((0, 1))
            step = (-1,0)
        end
        visited = Move(M, currentpos + CartesianIndex(step), currentpos, visited, debug) 
    elseif letter == '\\'
        if currentdirection == CartesianIndex((1,0))
            step  = (0, 1)
        elseif currentdirection == CartesianIndex((0,-1))
            step = (-1,0)
        elseif currentdirection == CartesianIndex((-1,0))
            step = (0,-1)
        elseif currentdirection == CartesianIndex((0, 1))
            step = (1,0)
        end
        visited = Move(M, currentpos + CartesianIndex(step), currentpos, visited, debug) 
    end
    
    return visited
end

function getEnergizedNum(M, startpos, previouspos)
    visited = Set()
    visited = Move(M, startpos, previouspos, visited, false)


    energized = [el[1] for el in visited]
    unique!(energized)
    energizedTiles = length(energized)
    return energizedTiles
end

fn = "input.txt"
fn = "test1.txt"


lines = readlines(fn)

M = reduce(vcat, permutedims.(collect.(lines)))
previouspos = CartesianIndex(1,0)
currentpos = CartesianIndex(1,1)
debug = false
visited = Set()
visited = Move(M, currentpos, previouspos, visited, debug)


energized = [el[1] for el in visited]
unique(energized) |> sort


currentpos = CartesianIndex((10,8))
previouspos = CartesianIndex((9,8))


#part 2
fn = "input.txt"
fn = "test1.txt"
lines = readlines(fn)
M = reduce(vcat, permutedims.(collect.(lines)))
sols = []
Msize = size(M)
for j in 1:Msize[2]#top and bottom
    push!(sols, getEnergizedNum(M, CartesianIndex((1,j)), CartesianIndex((0,j))))
    push!(sols, getEnergizedNum(M, CartesianIndex((Msize[1],j)), CartesianIndex((Msize[1]+1,j))))
end
for i in 1:Msize[1]
    push!(sols, getEnergizedNum(M, CartesianIndex((i,1)), CartesianIndex((i,0))))
    push!(sols, getEnergizedNum(M, CartesianIndex((i, Msize[2])), CartesianIndex((i, Msize[2]+1))))
end
maximum(sols)
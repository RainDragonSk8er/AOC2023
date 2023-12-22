using DataStructures
using Memoization
using AStarSearch

function vis(Mpad, visited)
    mvis = copy(Mpad)
    for pos in visited
        mvis[pos] = 'O'
    end
    return mvis
end

fn = "input.txt"
fn = "test1.txt"
lines = readlines(fn)
M = reduce(vcat, permutedims.(collect.(lines)))
maxi, maxj = size(M)

Mpad = Matrix{Any}(undef, maxi+2, maxj+2)
Mpad .= '#'
Mpad[2:end-1, 2:end-1] = M


startix = findfirst(x -> x == 'S', Mpad)


Q = Deque{Any}()
ends = Set()
push!(Q, (startix, 0))
pos = startix
maxsteps = 64
tiles = 0
debug = false
layers = Dict(0 => [pos])
while !isempty(Q)
    pos, step = popfirst!(Q)
    debug ? println("$pos at step $step") : nothing
    if step > maxsteps
        break
    end
    for dir in [(1,0), (0,1), (-1,0), (0,-1)]
        dir = CartesianIndex(dir)
        if !haskey(layers, step+1)
            layers[step+1] = []
        end
        if Mpad[pos + dir] != '#' && pos + dir ∉ layers[step+1]
            push!(layers[step+1], pos+dir)
            debug ? println("Pushing $(((pos + dir, step +1)))") : nothing
            if step+1 == maxsteps
                push!(ends, pos+dir)
            else
                push!(Q, (pos + dir, step +1))
            end
        end
    end
    tiles += 1
end
length(Q)
tiles
ends
display(vis(Mpad, ends))
######################################################################################################
#------------Part2-------------------
######################################################################################################
fn = "input.txt"
fn = "test1.txt"
lines = readlines(fn)
M = reduce(vcat, permutedims.(collect.(lines)))
maxi, maxj = size(M)

startix = findfirst(x -> x == 'S', M)


Q = Deque{Any}()
ends = Set()
push!(Q, (startix, 0))
pos = startix
maxsteps = 10 #26501365
tiles = 0
debug = false
layers = Dict(0 => [pos])
while !isempty(Q)
    pos, step = popfirst!(Q)
    debug ? println("$pos at step $step") : nothing
    if step > maxsteps
        break
    end
    for dir in [(1,0), (0,1), (-1,0), (0,-1)]
        pos = Tuple(pos)
        if !haskey(layers, step+1)
            layers[step+1] = []
        end
        newpos = (pos[1] + dir[1],pos[2] + dir[2] )
        if newpos[1] == 0
            newpos = (maxi, newpos[2])
        elseif newpos[1] == maxi+1
            newpos = (1, newpos[2])
        elseif (newpos[2]) == 0
            newpos = (newpos[1], maxj)
        elseif newpos[2] == maxj+1
            newpos = (newpos[1], 1)
        end
        newpos = CartesianIndex(newpos)
        if M[newpos] != '#' && newpos ∉ layers[step+1]
            push!(layers[step+1], newpos)
            debug ? println("Pushing $(((newpos, step +1)))") : nothing
            if step+1 == maxsteps
                push!(ends, newpos)
            else
                push!(Q, (newpos, step +1))
            end
        end
    end
    tiles += 1
end
length(ends)
display(vis(M, ends))
using Memoization
@memoize function ShortestPath(pos, dir, straightmoves, level)
    debug ? println("Pos:$pos, Dir:$dir, sm:$straightmoves, level:$level") : nothing
    bounds = [size(M)[1], size(M)[2]]
    if pos == bounds
        debug ? println("reached end") : nothing
        # error("reached end")
        return [], M[pos[1], pos[2]]
    end
    directions = [[1,0], [-1,0], [0,1], [0,-1]]
    possibledirections = directions
    setdiff!(possibledirections, [-dir])
    if straightmoves == straightmovesmax
        setdiff!(possibledirections, [dir])
    end
    if bounds in (possibledirections .+ Ref(pos))
        debug ? println("Adjacent to goal!") : nothing
        return [[bounds - pos]], M[pos[1], pos[2]] + M[bounds[1], bounds[2]]
    end

    choices = [[], []]
    for (i, step) in enumerate(possibledirections)
        if all([1,1] .<= pos + step .<= bounds)
            newstraightmoves = step == dir ? straightmoves + 1 : 1
            moves, val = ShortestPath(pos + step, step, newstraightmoves, level+1)
            debug ? println("Got something!pos:$pos") : nothing
            push!(choices[1], step)
            push!(choices[2], val)
        end
    end
    besti = argmin(choices[2])
    beststep = choices[1][besti]
    value = choices[2][besti] + M[pos[1], pos[2]]
    push!(moves, beststep)
    debug ? println("Done at pos:$pos, dir:$dir. Value:$value") : nothing
    error()
    return  moves, value
end

# moves, value = ShortestPath([1,1], [1,0], 1, 1)


#testing
bounds = [size(M)[1], size(M)[2]]
dir = [1,0]
pos = [12,13]
straightmoves = 1
step = [1,0]
[1,1] .<= pos + step .<= bounds
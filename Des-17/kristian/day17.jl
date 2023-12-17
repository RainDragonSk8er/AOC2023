using Memoization
using AStarSearch


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
state = start

#-
fn = "input.txt"
fn = "test1.txt"
fn = "test2.txt"

lines = readlines(fn)

global M = parse.(Int, reduce(vcat, permutedims.(collect.(lines))))

global debug = false
global straightmovesmin = 4
global straightmovesmax = 10
goal = State(size(M), (0,1), 1,straightmovesmax)
start = State((1,1), (1,0), 0, straightmovesmax)
result = astar(neighbors, start, goal, cost=cost, isgoal=isgoal)
result.cost
result.path
#tried 1365, 1362*
#AStarSearch
struct State
    pos::Tuple
    dir::Tuple
    straightmoves::Int
    maxstraightmoves::Int
end

function isgoal(state::State, goal::State)
    return all(state.pos .== goal.pos)
end

function neighbors(state::State)
    directions = [(1,0), (-1,0), (0,1), (0,-1)]
    possibledirections = directions
    setdiff!(possibledirections, [-1 .* state.dir])
    if state.straightmoves == straightmovesmax
        # debug ? println("cant move further!") : nothing
        setdiff!(possibledirections, [state.dir])
    end
    if straightmovesmin > state.straightmoves
        possibledirections = [state.dir]
    end
    neighbors =  []
    for step in possibledirections
        # debug ? println("nb:$(state.pos .+ step)") : nothing
        if step == state.dir
            steplength = 1
            newstraightmoves = state.straightmoves + 1
        else
            steplength = straightmovesmin
            newstraightmoves = 4
        end
        
        if !all((1,1) .<= state.pos .+ (steplength .* step) .<= size(M))
            continue
        end
        # newstraightmoves = step == state.dir ? state.straightmoves + 1 : 4
        nb = State(state.pos .+ (steplength .* step), step, newstraightmoves, state.maxstraightmoves)
        push!(neighbors, nb)
    end
    return neighbors
end

function cost(state::State, nb::State)
    i, j = nb.pos
    si, sj = state.pos
    istart = min(si,i)
    iend = max(si,i)
    jstart = min(sj,j)
    jend = max(sj,j)
    c = sum(M[istart:iend, jstart:jend]) - M[si, sj]
    return c
end
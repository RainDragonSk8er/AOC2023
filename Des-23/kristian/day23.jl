using AStarSearch
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
            newstraightmoves = straightmovesmin
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
fn = "input.txt"
fn = "test1.txt"
fn = "test2.txt"

lines = readlines(fn)

global M = parse.(Int, reduce(vcat, permutedims.(collect.(lines))))





#AStarSearch
#animation
using Plots
heatmap(M, colorbar=false, axis=false, xticks=nothing, yticks=nothing)
currentPlot = current()
yflip!(currentPlot)
anim = Animation()
for i in 2:length(result1.path)
    x1, y1 = result1.path[i].pos
    x2, y2 = result1.path[i-1].pos
    plot!([x1, x2], [y1, y2], legend=false, color=:green, lw=3)
    frame(anim)  # Capture each frame
end
for i in 2:length(result2.path)
    x1, y1 = result2.path[i].pos
    x2, y2 = result2.path[i-1].pos
    plot!([x1, x2], [y1, y2], legend=false, color=:blue, lw=3)
    frame(anim)  # Capture each frame
end
gif(anim, "path_animation.gif", fps = 25)  # Adjust fps (frames per second) as needed



 
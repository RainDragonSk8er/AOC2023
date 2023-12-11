fn = "test1.txt"
fn = "test2.txt"
fn = "input.txt"
lines = readlines(fn)
maxj = length(lines[1])
maxi = length(lines)

M = reduce(vcat, permutedims.(collect.(lines)))
MP = copy(M)
startpos = findfirst(M .== 'S')

function Nextposition(currentpos::CartesianIndex, previouspos::CartesianIndex)
    letter = M[currentpos]
    if letter == '|'
        if (currentpos - previouspos) == CartesianIndex((1,0))
            step  = (1, 0)
        else
            step = (-1,0)
        end
    elseif letter == '-'
        if (currentpos - previouspos) == CartesianIndex((0,1))
            step  = (0, 1)
        else
            step = (0,-1)
        end
    elseif letter == 'L'
        if (currentpos - previouspos) == CartesianIndex((1,0))
            step  = (0, 1)
        else
            step = (-1,0)
        end
    elseif letter == 'J'
        if (currentpos - previouspos) == CartesianIndex((1,0))
            step  = (0, -1)
        else
            step = (-1,0)
        end
    elseif letter == '7'
        if (currentpos - previouspos) == CartesianIndex((0,1))
            step  = (1, 0)
        else
            step = (0,-1)
        end
    elseif letter == 'F'
        if (currentpos - previouspos) == CartesianIndex((-1,0))
            step  = (0, 1)
        else
            step = (1,0)
        end
    end
    nextpos = currentpos + CartesianIndex(step)
    return nextpos
end

function FindFirstStep(startindex::CartesianIndex)
    pathup = false
    pathdown = false
    pathright = false
    pathleft = false
    upstep = CartesianIndex((-1,0))
    rightstep = CartesianIndex((0, 1))
    downstep = CartesianIndex((1, 0))
    leftstep = CartesianIndex((0, -1))
    if !(0 in Tuple(startindex + upstep)) && M[startindex + upstep] in Set(['|', '7', 'F'])
        pathup = true
        step =  upstep
    end
    if !(0 in Tuple(startindex + rightstep)) && M[startindex + rightstep] in Set(['-', 'J', '7'])
        pathright = true
        step = rightstep
    end
    if !(0 in Tuple(startindex + downstep)) && M[startindex + downstep] in Set(['|', 'J', 'L'])
        pathdown = true
        step = downstep
    end
    if !(0 in Tuple(startindex + leftstep)) && M[startindex + leftstep] in Set(['-', 'F', 'L'])
        pathleft = true
        step = leftstep
    end
    if pathup
        if pathright
            startChar = 'L'
        elseif pathdown
            startChar = '|'
        else
            startChar = 'J'
        end
    elseif pathright
        if pathdown
            startChar = 'F'
        elseif pathleft
            startChar = '-'
        end
    else
        startChar = '7'
    end
    M[startindex] = startChar
    return step 
end



firststep = FindFirstStep(startpos)
i = 1
currentpos = startpos + firststep 
previouspos = startpos
MP[startpos] = 'P'
while currentpos != startpos
    i += 1
    nextpos = Nextposition(currentpos, previouspos)
    previouspos = currentpos
    currentpos = nextpos
    MP[previouspos] = 'P'
end
i
farthest = Int(i/2)

# Part 2

tot = 0
for i in 1:maxi
    for j in 1:maxj
        if MP[i, j] == 'P'
            continue
        else
            # print("i=$i, j=$j")
            tiles = M[1:i-1, j]
            pathtiles = MP[1:i-1, j] .== 'P'
            pathpipes = tiles[pathtiles]
            pathscrossed = PathCrossings(pathpipes)
            # println(" crossed = $pathscrossed")
            if mod(pathscrossed, 2) == 1
                #odd number of crossings, means we started on inside
                tot += 1
            end
        end
    end
end
tot

function PathCrossings(pathpipes)
    if length(pathpipes) == 0
        return 0
    end
    crossings = 0
    vertcrossinglength = 0
    if pathpipes[end] == '-'
        crossings += 1
        vertcrossinglength = 1
    elseif pathpipes[end] == 'J'
        #incoming from left
        vertcrossinglength = 2
        while pathpipes[end-vertcrossinglength+1] == '|'
            vertcrossinglength += 1
        end
        if pathpipes[end-vertcrossinglength+1] == 'F'
            #crossing from left to right
            crossings += 1
        elseif pathpipes[end-vertcrossinglength+1] == '7'
            #in from left, up and out to left: no crossing
        else
            error("should not come here")
        end
    elseif  pathpipes[end] == 'L'
        #incoming from right
        vertcrossinglength = 2
        while pathpipes[end-vertcrossinglength+1] == '|'
            vertcrossinglength += 1
        end
        if pathpipes[end-vertcrossinglength+1] == '7'
            #crossing from right to left
            crossings += 1
        elseif pathpipes[end-vertcrossinglength+1] == 'F'
            #in from right, up and out to right: no crossing
        else
            error("should not come here")
        end
    end
    crossings += PathCrossings(pathpipes[1:end-vertcrossinglength])
    return crossings
end
#=
uptiles = M[1:i-1, j]
uptilesP = MP[1:i-1, j]
uppaths = sum( (uptiles .∈ Ref(['-', 'F', '7'])) .& (uptilesP .== 'P') )
downtiles = M[i+1:maxi, j]
downtilesP = MP[i+1:maxi, j]
downpaths = sum( (downtiles .∈ Ref(['-', 'L', 'J'])) .& (downtilesP .== 'P') )

righttiles = M[i, j+1:maxj]
righttilesP = MP[i, j+1:maxj]
rightpaths = sum( (righttiles .∈ Ref(['|', '7', 'J'])) .& (righttilesP .== 'P') )

lefttiles = M[i, 1:j-1]
lefttilesP = MP[i, 1:j-1]
leftpaths = sum( (lefttiles .∈ Ref(['|', 'F', 'L'])) .& (lefttilesP .== 'P') )

=#
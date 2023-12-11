fn = "test1.txt"
fn = "input.txt"
lines = readlines(fn)

M = reduce(vcat, permutedims.(collect.(lines)))
maxi = length(lines)
maxj = length(lines[1])
i = 1
while i <= maxi
    if all(M[i, :] .== '.')
        M = [M[1:i, :]; reshape(M[i, :], (1,maxj)); M[i+1:end, :]]
        maxi += 1
        i += 2
    else
        i += 1
    end
end
M
maxi, maxj = size(M)
j = 1
while j <= maxj
    if all(M[:, j] .== '.')
        M = [M[:, 1:j]  reshape(M[:, j], (maxi,1)) M[:, j+1:end]]
        maxj += 1
        j += 2
    else
        j += 1
    end
end
M
galaxies = (findall(x->x=='#', M))
L = length(galaxies)

pathlengths = zeros(Int64, (L, L))
for (i, galaxy) in enumerate(galaxies)
    for j in i+1:L
        diff = galaxies[i] - galaxies[j]
        pathlengths[i, j] = abs(diff[1]) + abs(diff[2])
    end
end
pathlengths
sum(pathlengths)

#part 2
multiplier = 1_000_000
M = reduce(vcat, permutedims.(collect.(lines)))
maxi = length(lines)
maxj = length(lines[1])

emptyrows = []
for i in 1:maxi
    if all(M[i, :] .== '.')
        push!(emptyrows, i)
    end
end

emptycols = []
for j in 1:maxj
    if all(M[:, j] .== '.')
        push!(emptycols, j)
    end
end


galaxies = (findall(x->x=='#', M))
L = length(galaxies)
pathlengths = zeros(Int64, (L, L))
for (i, galaxy) in enumerate(galaxies)
    for j in i+1:L
        i1 = galaxies[i][1]
        i2 = galaxies[i][2]
        j1 = galaxies[j][1]
        j2 = galaxies[j][2]
        rowsbetween = sum(emptyrows .∈ Ref(min(i1, j1):max(i1,j1)))
        colsbetween = sum(emptycols .∈ Ref(min(i2, j2):max(i2,j2)))
        diff = galaxies[i] - galaxies[j]
        pathlengths[i, j] = abs(diff[1]) + abs(diff[2]) + rowsbetween*(multiplier-1) + colsbetween*(multiplier -1)
    end
end
pathlengths
sum(pathlengths)
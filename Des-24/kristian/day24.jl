using DataStructures
fn = "input.txt"
fn = "test1.txt"
lines = readlines(fn)


particles = []
for line in lines
    pos, vel = split(line, '@')
    px, py, pz = parse.(Float64, split(pos, ','))
    vx, vy, vz = parse.(Float64, split(vel, ','))
    push!(particles, [[px, py, pz], [vx, vy, vz]])
end

lo = 200000000000000
hi = 400000000000000
L = length(particles)
intersections = 0
for i in 1:L
    println(i)
    pi = particles[i][1]
    vi = particles[i][2]

    for j in i+1:L
        pj = particles[j][1]
        vj = particles[j][2]

        A = [vi[1] -vj[1]; vi[2] -vj[2]]
        b = [pj[1]-pi[1];pj[2]-pi[2]]

        det = -vi[1]*vj[2] + vj[1]*vi[2] 
        if det ≈ 0
            # println("singular matrix")
            continue
        end
        tsol = A\b
        xint = pi[1] + tsol[1]*vi[1]
        yint = pi[2] + tsol[1]*vi[2]
        if lo <= xint <= hi && lo <= yint <= hi && all(tsol .>0)
            # println("intersection between $i and $j at x=$xint, y=$yint. t1=$(tsol[1]), t2=$(tsol[2])")
            intersections += 1
        end
    end
end
intersections

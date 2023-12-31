using DataStructures
using NonlinearSolve
using Symbolics
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
    # println(i)
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

##########################################################################
#part 2
##########################################################################

##############
done = 0
i = 0
p1 = particles[1][1]
v1 = particles[1][2]
    
@variables x, y, z, vx, vy, vz

eqs = []

for particle in particles
    px, py, pz = particle[1][1], particle[1][2], particle[1][3]
    pvx, pvy, pvz = particle[2][1], particle[2][2], particle[2][3]    
    push!(eqs, Eq(px + (x-px)/(pvx-vx) * pvx, x + (x-px)/(pvx-vx) * vx ))
    push!(eqs, Eq(py + (y-py)/(pvy-vy) * pvy, y + (y-py)/(pvy-vy) * vy ))
    push!(eqs, Eq(pz + (z-pz)/(pvz-vz) * pvz, z + (z-pz)/(pvz-vz) * vz ))
    
end
#############


 


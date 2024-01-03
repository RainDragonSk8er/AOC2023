using DataStructures, Primes

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
function all_divisors(prime_factors)
    divisors = [1]
    for (prime, exp) in prime_factors
        current_length = length(divisors)
        for i in 1:exp
            for j in 1:current_length
                push!(divisors, divisors[j] * prime^i)
            end
        end
    end
    return sort(unique(divisors))
end
##############
L = length(particles)
velmatchinfo = []
index = 3
for i = 1:L
    pi = Int(particles[i][1][index])
    vi = Int(particles[i][2][index])
    for j = i+1:L
        vj = Int(particles[j][2][index])
        pj =  Int(particles[j][1][index])

        if vi == vj
            println("velocity match= $vj for i=$i, j=$j")
            push!(velmatchinfo, [pi-pj, vj])
        end
    end
end
velmatchinfo
#############
possibleVels = all_divisors(factor(-1*abs(velmatchinfo[1][1]))) .+ velmatchinfo[1][2]
for i = eachindex(velmatchinfo[2:end])
    f = factor(-1*abs(velmatchinfo[i][1]))
    vels = all_divisors(f) .+ velmatchinfo[i][2]
    intersect!(possibleVels, vels)
end
possibleVels
#vx = -193, vy= -230, vz=218
#########################BRUTE FORCE###############################
v = [-193, -230, 218]
maxt = 10_000_000
t = -1*maxt
p1 = particles[1][1]
v1 = particles[1][2]
done=false
while !done && t <= maxt
    p = p1+ t*v1 - t*v
    j = 2
    for j in 2:L
        pj = particles[j][1]
        vj = particles[j][2]
        tj = (p[1]- pj[1])/(vj[1]-v[1])
        if (p[2]- pj[2])/(vj[2]-v[2]) == tj && (p[3]- pj[3])/(vj[3]-v[3]) == tj
            println("match for t = $t, j=$j")
            done = true
        else
            done=false
            break
        end
    end

    t+=1
end
done
#####################################################################
A = zeros((900, 303))
b = zeros(900)
V = [-193, -230, 218]
for i = 1:300
    Pi = particles[i][1]
    Vi = particles[i][2]
    for j = 1:3
        A[3*(i-1)+j, j] = 1
        A[3*(i-1)+j, 3+i] = V[j]-Vi[j]
        b[3*(i-1)+j] = Pi[j]
        if V[j]==Vi[j]
            println("equalvelocities!i = $i, j=$j")
        end
    end
end
sol = A\b
Int(sum(sol[1:3]))

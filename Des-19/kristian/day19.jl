using DataStructures
using AStarSearch
using Memoize
fn = "input.txt"
fn = "test1.txt"
lines = readlines(fn)
#M = reduce(vcat, permutedims.(collect.(lines)))
divider = findfirst(x-> x =="", lines)
block1 = lines[1:divider-1]
block2 = lines[divider+1:end]

debug = false

letterToIndex=Dict('x' => 1, 'm'=>2, 'a'=>3, 's'=>4)
Workflows = Dict()
for wf in block1
    parts = split(wf, "{")
    key = parts[1]
    # fnrules = Vector{Any}(undef, 5)
    rules = split(parts[2][1:end-1], ',')
    fnrules = []
    for rule in rules
        if ':' ∉ rule
            push!(fnrules, rule)
        else
            expr, dest = split(rule, ':')
            index = letterToIndex[expr[1]]
            if expr[2] == '<' 
                op = < 
            else
                op = >
            end
            num = parse(Int, expr[3:end])
            # fn = x -> op(x[index], num)
            fn = [index, op, num]
            push!(fnrules, [fn, dest])
        end
    end
    Workflows[key] = fnrules
end

parts = []
for part in block2
    part = strip(part, ['{', '}'])
    splitted = split(part, ',')
    partvec = Vector{Int}(undef, 4)
    for (i, t) in enumerate(splitted)
        partvec[i] = parse(Int, t[3:end])
    end
    push!(parts, partvec)
end
parts
#parts are now vectors
R = []
A = []
for part in parts
    wf = "in"
    done = false
    debug ? println("Part: $part") : nothing
    while !done
        debug ? println("Going to: $wf") : nothing
        fnrules = Workflows[wf]
        rulesnum = length(fnrules)
        for (i, rule) in enumerate(fnrules)
            if i == rulesnum #only dest, last entry
                # debug ? println("last in rule, $rule") : nothing
                wf = rule
            else
                expr = rule[1]
                dest = rule[2]
                ix = expr[1]
                op = expr[2]
                num = expr[3]
                if op(part[ix], num)#rule[1](part)
                    wf = dest   
                    break
                end
                
            end
        end
        if wf == "A"
            debug ? println("found A, Done") : nothing
            push!(A, part)
            done = true
        elseif wf == "R"
            debug ? println("found R, Done") : nothing
            push!(R, part)
            done = true
        end
    end
end
A
R
sum(sum(A))

wfA =  Dict()
wf = "in"
v = [(1,4000), (1,4000), (1,4000), (1,4000)]
debug = false
@time totcombs = findCombs(wf, v)



function findCombs(wf, v)
    combinations = 0
    if wf == "A"
        return combs(v)
    elseif wf =="R"
        return 0
    end
    rules = Workflows[wf]
    rulesnum = length(rules)
    locv = copy(v)
    done = false
    for (i, rule) in enumerate(rules) 
        sendv = copy(locv)
        if i == rulesnum #only dest, last entry
            # debug ? println("last in rule, $rule") : nothing
            combinations += findCombs(rule, sendv)
        else
            expr = rule[1]
            dest = rule[2]
            ix = expr[1]
            op = expr[2]
            num = expr[3]
            if (op == <) && sendv[ix][1] < num
                start = sendv[ix][1]
                stop = sendv[ix][2]
                if stop >= num
                    locv[ix] = (num, stop)
                    stop = num-1
                else
                    done = true
                end
                sendv[ix] = (start, stop)
                debug ? println("Sending $sendv to $dest") : nothing
                combinations += findCombs(dest, sendv)
            elseif (op == >) && sendv[ix][2] > num
                start = sendv[ix][1]
                stop = sendv[ix][2]
                if start <= num
                    locv[ix] = (start, num)
                    start = num+1
                else
                    done = true
                end
                sendv[ix] = (start, stop)
                debug ? println("Sending $sendv to $dest") : nothing
                combinations += findCombs(dest, sendv)
            end            
        end
        if done
            break
        end
    end
    return combinations
end

function combs(v)
    c = 1
    for r in v
        c*=(r[2]-r[1] +1)
    end
    return c
end
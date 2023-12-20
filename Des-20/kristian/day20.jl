using DataStructures
using AStarSearch
using Memoization

function SetupNodes(fn)
    lines = readlines(fn)
    nodes = Dict() #name -> [type, downstreamneighbors, upstreamneighbors, state]. upstream:dict(preneighbor -> mem)

    for i in 1:2
        for line in lines[1:end]
            node, nbs = split(line, " -> ")
            neighbors = split(nbs, ", ")
            type = node[1] #broadcaster getrs type b
            name = node[2:end]
            name == "roadcaster" ? name = "broadcaster" : nothing
            for nb in neighbors
                if haskey(nodes, nb)
                    nodes[nb][memix][name] = lowPulse
                end
            end
            if !haskey(nodes, name)
                nodes[name]= [type, neighbors, Dict{Any, Bool}(), 0]
            end
        end
    end
    return nodes
end

function SendPulse(;stopon= (x,y) -> false)
    highs = 0
    lows = 1
    pulses = Deque{Tuple}()
    pushfirst!(pulses, ("broadcaster", 0))
    while !isempty(pulses)
        pulse = popfirst!(pulses)
        # if pulse == stopon
        #     return -1, -1
        # end
        from = pulse[1]
        pulseType = pulse[2]
        node = nodes[from]
        
        for nb in node[nbix]#[end:-1:1]
            debug ? println("$from -> $pulseType -> $nb") : nothing
            pulseType == 1 ? highs+=1 : lows += 1
            #que pulse to nb, to start of deque
            if stopon(nodes, (nb,pulseType))
                return -1, -1
            end
            if !haskey(nodes, nb)
                continue
            elseif nodes[nb][typeix] == '%'
                if pulseType == highPulse
                    continue
                elseif nodes[nb][stateix] == 1#on, dont send pulse
                    nodes[nb][stateix] = 0
                    sendpulse = lowPulse
                else
                    nodes[nb][stateix] = 1
                    sendpulse = highPulse
                end
            elseif nodes[nb][typeix] == '&'
                nodes[nb][memix][from] = pulseType
                # debug ? println("$nb memory now $(nodes[nb][memix])") : nothing
                # nodes["ql"][memix]["mf"] == 1 ? println("$nb memory now $(nodes[nb][memix])") : nothing
                if all(values(nodes[nb][memix]) .== 1)#if all in memory are high, send low
                    sendpulse = lowPulse
                else
                    sendpulse = highPulse #else send high
                end
            end
            newpulse = (nb, sendpulse)
            # debug ? println("Pushing pulse $newpulse") : nothing
            push!(pulses, newpulse)
        end
    end
    return highs, lows
end
fn = "input.txt"
fn = "test1.txt"
fn = "test2.txt"
lines = readlines(fn)
debug = false
#M = reduce(vcat, permutedims.(collect.(lines)))
nbix = 2
typeix = 1
memix = 3
stateix = 4
lowPulse = 0
highPulse = 1
# nodes= SetupNodes(fn)
# @run SendPulse(("", 2))

tothi = 0
totlo = 0
numpulses = 1000
nodes = SetupNodes(fn)
for i in 1:numpulses
    hi, lo = SendPulse()
    tothi += hi
    totlo += lo
end
tothi*totlo

#part 2 brute force
j = 0
nodes = SetupNodes(fn)
hi = 0
stopfn = (n, p) -> n["ql"][memix]["fz"] == 1
while hi != -1 && j< 50_000
    hi, lo = SendPulse()
    if nodes["ql"][memix]["mf"] == 1
        println("mf")
    end
    j+=1
end
j
#part 2 smart??
nodes = SetupNodes(fn)
hi, lo = SendPulse()
states = FindCycle(fn, "mf")

function FindCycle(fn, target)
    nodes = SetupNodes(fn)
    states = Dict(nodes[target] => [])
    done = false
    maxi = 4000
    i = 0
    while !done && i< maxi
        hi, lo = SendPulse()
        config = copy(nodes[target])
        if !haskey(states, config)
            states[nodes[target]]= 0
        else
            push!(states[nodes[target]], i)
        end
        i += 1
    end
    return states
end

# function PressesToPulse(pulse)
#     locnodes = copy(nodes)
#     name = pulse[1]
#     pulseType = pulse[2]
#     if locnodes[name][typeix] == '&'
#         nbs = keys(locnodes[name][memix])
#         pressesToPreNbs = [PressesToPulse() ]
# end

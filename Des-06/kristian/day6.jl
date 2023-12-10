# filename = "test1.txt"
filename = "input.txt"
lines = readlines(filename)
part = 2

if part ==1
    times =  lstrip(lines[1], collect("Time:")) |> lstrip
    times = split(times, ' ')
    times = filter(!isempty, times)
    times = parse.(Float64, times)

    racedistances =  lstrip(lines[2], collect("Distance:")) |> lstrip
    racedistances = split(racedistances, ' ')
    racedistances = filter(!isempty, racedistances)
    racedistances = parse.(Float64, racedistances)
else
    times = parse(Float64, filter(isdigit, lines[1]))
    racedistances = parse(Float64, filter(isdigit, lines[2]))
end



L = length(times)

tot = 1
for i in 1:L
    T = times[i]
    Dr = racedistances[i] + 1e-8

    lowerBound = max(ceil((T - sqrt(T^2 - 4*Dr))/2), 0)
    upperbound = min(floor((T + sqrt(T^2 - 4*Dr))/2), T)

    tot *= (upperbound - lowerBound + 1)
end
tot
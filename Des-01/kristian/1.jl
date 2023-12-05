lines = readlines("1/input.txt")
numbers = zeros(Int8, length(lines))
for (i, line) in enumerate(lines)
    digs = filter(isdigit, line)
    num1 = digs[1]
    num2 = digs[end]
    num = parse(Int, num1*num2)
    numbers[i] = num
end
tot = sum(numbers)

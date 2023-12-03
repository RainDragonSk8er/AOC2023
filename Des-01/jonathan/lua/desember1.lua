-- Lua is 1 indexed!!!! WTF
PATH = "Des-01/jonathan/input/puzzle_input.txt"
TEST_PATH_1 = "Des-01/jonathan/input/test1_input.txt"
TEST_PATH_2 = "Des-01/jonathan/input/test2_input.txt"

StrToInt_dict = {
    ["one"] = "1",
    ["two"] = "2",
    ["three"] = "3",
    ["four"] = "4",
    ["five"] = "5",
    ["six"] = "6",
    ["seven"] = "7",
    ["eight"] = "8",
    ["nine"] = "9"
}

local function func1 (path)
    local cum_sum = 0
    local i_file = io.open(path, "r")
    if i_file == nil then
        error("Whoops, the input path is nil!", 1)
    end
    for line in i_file:lines("l") do
        local numbers = {}
        for num in string.gmatch(line, "%d") do
            numbers[#numbers+1] = num
            
        end
        local num_str = numbers[1]..numbers[#numbers]
        local num_int = tonumber(num_str, 10)
        cum_sum = cum_sum + num_int
        
    end
    return cum_sum
end

function Dump_table(o)
    if type(o) == 'table' then
       local s = '{ '
       for k,v in pairs(o) do
          if type(k) ~= 'number' then k = '"'..k..'"' end
          s = s .. '['..k..'] = ' .. Dump_table(v) .. ','
       end
       return s .. '} '
    else
       return tostring(o)
    end
 end
 

function Find_n_replace (entry, hash_map, reverse)
    local found_keys = {}
    if reverse then
        local rev_hash_map = {}
        for key, value in pairs(hash_map) do
            rev_hash_map[string.reverse(key)] = hash_map[key]
        end
        hash_map = rev_hash_map
        entry = string.reverse(entry)
    end

    for key, value in pairs(hash_map) do
        found_keys[key] = string.find(entry, key)
        if found_keys[key] ~= nil then
            if found_keys["min"] == nil then
                found_keys["min"] = key
            end         
            if found_keys[key] < found_keys[found_keys["min"]] then 
                found_keys["min"] = key
            end    
        end
    
    end
    -- print("\t\t", Dump_table(found_keys))
    if found_keys["min"] ~= nil then
        return_string = string.gsub(entry, found_keys["min"], hash_map[found_keys["min"]])    
    else
        return_string = entry
    end
    if reverse then
        return_string = string.reverse(return_string)
    end

    return return_string
    
end

local function func2 (path, hash_map)
    local cum_sum = 0
    local i_file = io.open(path, "r")
    if i_file == nil then
        error("Whoops, the input path is nil!", 1)
    end
    for line in i_file:lines("L") do
        local entry = string.lower(line)
        if entry:match("^%l+.+\n$") then --pattern match: "^%w+$"
            local changed_string = Find_n_replace(line, StrToInt_dict, false)
            -- print(line, "->", changed_string)
            entry = string.gsub(entry, line, changed_string)    
        end
        if entry:match("^.+%l+\n$") then --pattern match: "^%w+$"
            local changed_string = Find_n_replace(entry, StrToInt_dict, true)
            -- print("\t", entry, "->", changed_string)
            entry = string.gsub(entry, entry, changed_string)    
        end
    
        local numbers = {}
        for num in string.gmatch(entry, "%d") do
            numbers[#numbers+1] = num        
        end
        local out_str = {string.gsub(entry, "\n", "")}
        -- print("Processed string:", Dump_table(out_str))
        -- print("numbers table:", Dump_table(numbers))
        local num_str = numbers[1]..numbers[#numbers]
        -- print("Final value", num_str)
        local num_int = tonumber(num_str, 10)
        cum_sum = cum_sum + num_int
    end
    return cum_sum
end

local answer_1 = func1(PATH)
local answer_2 = func2(PATH, StrToInt_dict)
print("The sum of numbers for question 1 is:", answer_1)
print("The sum of numbers for question 2 is:", answer_2)

#include <iostream>
#include <fstream>
#include <vector>
#include <string>
#include <filesystem>
#include <regex>

std::filesystem::path PATH = "Des-01/jonathan/input/puzzle_input.txt";
std::filesystem::path TEST_1_PATH = "Des-01/jonathan/input/test1_input.txt";
std::filesystem::path TEST_2_PATH = "Des-01/jonathan/input/test2_input.txt";

u_int64_t readFile(std::filesystem::path myFile) {
    std::ifstream iFile (myFile.string());
    std::string line;
    u_int64_t sum = 0; 
    if (iFile.is_open()) {
        int elf_cal_count;
        while(std::getline(iFile, line)) {
            // Recurring capture gropus are not allowed only one value per declared capture group!
            // |---> Only the last match is captured!
            // Next time just loop over the string, and determine if a char is digit!
            std::regex my_regex = std::regex(R"((\d))", std::regex_constants::ECMAScript);
            bool first_bool = true;
            std::smatch matches;
            std::string str = line;
            std::vector<std::string> line_vec = {}; 
            while (std::regex_search(str, matches, my_regex)) {
                if (first_bool) {
                    std::cout << "\t Oh my, we found the regex!\n";
                    std::cout << "\t\t In: " << matches[0].str() << "\n";
                    first_bool = false;
                };
                
                line_vec.push_back(matches[1].str());

                std::cout << "\t\t Match["<< std::to_string(1) << "]: " << matches[1] << "\n";
                str = matches.suffix().str();
            };
            std::string my_first_int_str = line_vec.at(0);
            std::string my_last_int_str = line_vec.at(line_vec.size() - 1);
            u_int8_t my_int = std::stoi(my_first_int_str + my_last_int_str);
            std::cout << std::to_string(my_int) << "\n";
            
            sum += my_int;
        };
        iFile.close();
    };
    return sum;
};


int main() {
    u_int64_t answer_1_sum = readFile(PATH);
    std::cout << "This is my sum for question 1: " << std::to_string(answer_1_sum) << "\n";
    return 0;
};
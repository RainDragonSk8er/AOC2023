#include <iostream>
#include <fstream>
#include <vector>
#include <string>
#include <filesystem>

struct elf
{
    int elf_nr;
    int elf_calories;
};


void myFunc() {
    std::ofstream myFile ("input/dummy_dile.txt");
    if (myFile.is_open()) {
        myFile << "This is the first line!\n";
        myFile << "This is the second line!\n";
        myFile.close();
    };
};

elf readFile(std::filesystem::path myFile) {
    std::ifstream iFile (myFile.string());
    std::string line;
    int highest_cal_count = 0;
    int highest_cal_elf = 0;
    int elf_counter = 0;
    if (iFile.is_open()) {
        int elf_cal_count;
        while(std::getline(iFile, line)) {
            if (line != "\n"){
                int line_count;
                line_count << std::stoi(line);
                elf_cal_count += line_count;
            } else if (line == "\n") {
                if (elf_cal_count >= highest_cal_count) {
                    highest_cal_count = elf_cal_count;
                    highest_cal_elf = elf_counter;
                };
                ++elf_counter;
            } else {
                break;
            };
            
            
        };
        iFile.close();
        elf bestElf;
        bestElf.elf_nr = highest_cal_elf;
        bestElf.elf_calories = highest_cal_count;
        return bestElf;
    } else {
        std::cout << "There is no such file: " << myFile;
        elf bestElf;
        bestElf.elf_calories = -1;
        bestElf.elf_nr = -1;
        return bestElf;
    };
};


int main() {

    myFunc();
    elf bestElf = readFile("input/dummy_dile.txt");
    std::cout << "This is the best elf: \n\t Elf number: " << bestElf.elf_nr << "\n\t Elf calories: " << bestElf.elf_calories; 
    return 0;
};

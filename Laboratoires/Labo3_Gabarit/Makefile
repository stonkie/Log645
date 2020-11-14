CC = mpic++
CFLAGS = -Wall -fPIC -m64 -O3 -std=c++14
LFLAGS = -Wall -fPIC -m64

BUILD_DIR = build
SRC_DIR = src

SRC = $(shell find $(SRC_DIR) -name "*.cpp")
OBJ = $(patsubst $(SRC_DIR)/%.cpp,$(BUILD_DIR)/%.o,$(SRC))

EXE = lab3

RM = rm -r
MKDIR_P = mkdir -p

all: $(OBJ)
	$(CC) $(LFLAGS) -o $(EXE) $^

clean:
	$(RM) $(BUILD_DIR) $(EXE)

$(BUILD_DIR)/%.o: $(SRC_DIR)/%.cpp
	$(MKDIR_P) $(dir $@)
	$(CC) $(CFLAGS) -c $< -o $@

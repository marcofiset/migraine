# Migraine - Unfuck Your Brain

#### A set of projects related to [Brainfuck][1]

## What is it / What will it be

Migraine is a Brainfuck interpreter, debugger and will eventually feature a 
programming language that compiles down to Brainfuck. I created this project for 
learning purposes only and I have a lot of fun writing it. It is still in early development.

### Migraine - The custom Brainfuck interpreter

#### What currently works

- Fully functional interpreter which uses integers as memory cells.

#### Plans for the near future

##### `;` and `:` operations

`,` and `.` respectively allow to input and output integers. `;` and `:` will allow the input and output of strings.
String management in Brainfuck is such a pain in the neck that I will make it easier this way. Call me lazy, I don't care!

### Migraine - The Brainfuck debugger

#### What currently works

- Load/save a Brainfuck program from/to a file
- Execute the program (!)
- Supply input to the program as we go
- Pre-supply inputs to the program
- Display the output

#### Plans for the near future

- Step-by-step execution
- Display the memory cells and their values
- Live editing of the memory cells

### Migraine - The programming language

#### What currently works

- Grammar for simple mathematical expressions (numbers, `+ - * /` operators, parenthesis and operator precedence, no variable support yet)
- Abstract Syntax Tree generation
- Provides an interface for AST manipulation (through the Visitor pattern)

#### Plans for the near future

- Variable assignment
- Functions
- Conditions

[1]: http://en.wikipedia.org/wiki/Brainfuck
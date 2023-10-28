# luavim
Open source lua editor made in C# and JS.

# commands
* mkfile ... [ex: mkfile test.lua]
* createf ... [ex: createf C:\Users\%username here%\Documents\luavim], {create a new project, ...}
* loadf ... [ex: loadf C:\Users\%username here%\Documents\luavim], {loads a previously created project, ...}
* cdir ... [ex: cdir examplefolder], {changes main directory to selected folder, ...}
* mkfolder ... [ex: mkfolder examplefolder], {creates a new folder, ...}
* main ... [ex: main], {goes back to `src` directory within the project, ...}
* openf ... [ex: openf test.lua], {opens a text file to write in it, ...}
* relf ... [ex: relf], {writes the input taken from the editor straight to the file you used openf on, ...}
* ls ... [ex: ls], {lists all files within current folder, ...}

# config commands
* `#cf > ~/root/lv2.sidebar.select_on_click` {makes the sidebar useful, goes from terminal based to a VS Code type of editor, ...}

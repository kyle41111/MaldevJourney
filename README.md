My maldev journey!

My journey and progress through malware dev for redteaming purposes.
Learning Offensive cpp/c# for rtl/threat emulation. This will be a repo of my beginner projects while I attempt to go further in Offensive tooling/evasion.

I currently am using this library:
https://github.com/JustasMasiulis/inline_syscall

You will need to right click the project and go to properties>Platform Toolset LLVM(clang-cl). This can be downloaded through visual studio.
![image](https://user-images.githubusercontent.com/26053422/210104832-d5f87c98-bf62-4bc8-964f-56808d438f6c.png)

Additionally, you will find the header files under the /libs directory on this github. you will need to add the directory housing the header files as external include directories.
![image](https://user-images.githubusercontent.com/26053422/210105049-eaf26dff-2515-4e87-8e20-0e6630e9680f.png)



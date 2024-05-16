# Load Environment
Load environment variables from a file

- This program is designed to be used in a batch script. (.bat .cmd)
- doesn't directly set the variable, but outputs the commands to do so.

### Usage ```loadenv.exe <cfg> <switches>```
  

##### To set variables in the current environment, use a FOR LOOP in batch to 'catch the output'
```
for /f "delims=" %%k in ('loadenv.exe <cfg> <switches>') do ( %%k )
```

![Screenshot 2024-05-16 111037](https://github.com/tommojphillips/LoadEnvironment/assets/39871058/c0113450-2804-43db-9052-2ada93f40591)

##### To suppress the std error output, redirect it to nul.
```
for /f "delims=" %%k in ('loadenv.exe <cfg> <switches> 2^>nul') do ( %%k )
```

##### To log the std error, redirect it to a file.
```
for /f "delims=" %%k in ('loadenv.exe <cfg> <switches> 2^>error.txt') do ( %%k )
```

##### Comments can be used in the cfg file, they must start with ```;```

 ```; this is a comment```

 ##### To define a variable in the cfg file, use the following format:

```VAR=VAL```

##### To undefine a variable in the cfg file, use the following format:

```VAR=```

##### To expand a variable, use the following format:
- vars you want to expand must be enclosed in ```%```

```VAR=%OTHER_VAR%```

##### Variables from previous lines in the cfg file can be used to define new variables. EG:

```
PREV_VAR=Testing
NEXT_VAR=%OTHER_VAR% 123
```
- ```%NEXT_VAR%``` will be set to ```Testing 123```


### Switches 
- -q    - Suppress all output except for errors.

### Example cfg file ( .ini )
![Screenshot 2024-05-16 105717](https://github.com/tommojphillips/LoadEnvironment/assets/39871058/2f0653cb-e1bb-4c69-9f68-40d4bbddf026)


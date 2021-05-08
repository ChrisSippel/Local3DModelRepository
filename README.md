# Local3DModelRepository
A UI tool for managing all of our 3D models

## Table of contents
1. [Purpose](#Purpose)
2. [Notes](#Notes)
3. [Usage](#Usage)
  1. [Requirements](#Requirements)

## Purpose
The purpose of Local3dModelRepository (L3MR or lemur) is to make your life eaiser. As your collection of models exands, your method of tracking becomes more and more convaluded. The goal of Local3dModelRepository is to give you a single application that can keep track of all of the information about your models!

## Notes
L3MR is still in early development, and as such it's possible you will find bugs. If you do, please don't hesitate to log them. Or, if you're a developer, feel free to fix the bug, and push up a pull request!

If you would like to use the code within the tool, please feel free as it is licensed under the MIT license.

## Usage
Alright, enough of the boring stuff, how do I use it?

### Requirements
Operating System: Windows 7+

### Installation
1. Download the latest version of the tool. You can find find all releases of L3MR here <https://github.com/ChrisSippel/Local3DModelRepository/releases/>
2. Extract the .zip to a location you'll remember

### Using the tool
#### Getting Started
1. Start the tool by double clicking the Local3DModelRepository.exe file. The following window will appear.
![image](https://user-images.githubusercontent.com/6963296/117552417-1d77e600-b019-11eb-8dab-f8f25825ec1a.png)
2. The next step is to load all of your models into Local3dModelRepository. This can be done by clicking on "Open Folder" in the top left of Local3dModelRepository
![image](https://user-images.githubusercontent.com/6963296/117552453-616aeb00-b019-11eb-8f55-b2c9002bd7fb.png)
3. Then select the folder that contains all of your models. Local3dModelRepository will automatically go through all files and folders that are contained within the folder you select to find all files that end with .stl
4. Once Local3dModelRepository has found all 3D models, you will see all models that it supports on the left hand side
![image](https://user-images.githubusercontent.com/6963296/117552583-53699a00-b01a-11eb-908f-8a2db2545eef.png)
5. You can then click on a model on the left hand side to open that model. Note, depending on how big or detailed the model is, it can take a little while to open.
![image](https://user-images.githubusercontent.com/6963296/117552596-6b411e00-b01a-11eb-8a99-7239441e0f8a.png)
6. You can now manipulate the model by using the right mouse button, middle mouse button, and scrolling your mouse wheel
![image](https://user-images.githubusercontent.com/6963296/117552632-a17e9d80-b01a-11eb-94e7-95102a9596ec.png)

#### Tags
Once your models are loaded, you can add tags to them. You can use these tags for filtering your collection of models

1. You can add tags to a model by clicking on the Add Tag(s) button to the right of L3MR
![image](https://user-images.githubusercontent.com/6963296/117552725-397c8700-b01b-11eb-89a9-3c7614d3b6bb.png)
2. A window will pop-up that gives you the option to add tags to your currently selected model. You can add them in the text box, with each tag being separate by a space. The following picture shows how to add the tags "32mm", "scenary", and "no_base". Once you have the tags you like, click on the Add Tag(s) button.
![image](https://user-images.githubusercontent.com/6963296/117552770-6df04300-b01b-11eb-8598-a08cd5356d91.png)
3. You will then see the tags you added under the Assigned Tags section of the window. You will also see your tags under the Available Tags section. L3MR remembers all of the tags that you have assigned to your models. This way you can quick assign tags to models, if you've already used that tag. If you want to remove a tag for a model you can click on the 'x' to the right of the tag to remove it.
![image](https://user-images.githubusercontent.com/6963296/117552896-24ecbe80-b01c-11eb-9457-3d1f43cc563f.png)
4. If you want to save the changes you have made, then click on the Save & Close button. If you'd like to not save any changes you've made, click on the Close without saving button.
5. You will now see all tags that have been assigned to your model on the right hand side of L3MR. If you wish to remove any tag(s) from the model you can click on them, and click the Remove Tag(s) button
![image](https://user-images.githubusercontent.com/6963296/117552993-dd1a6700-b01c-11eb-9488-77e494939c49.png)

##### Inclusive Filters
If you want to only show models with specific tags, you can use the Tags to require in filter drop down in the top left of L3MR. This drop down contains all of the tags that have been applied to your models. To turn on a tag filter, just click on the checkbox. To turn it off, do the same thing. The filter can only add tags. Meaning if you want to filter models that are 32mm, and scenary, you can. But if you want to filter models that are 32mm or are scenary, you cannot
![image](https://user-images.githubusercontent.com/6963296/117553109-81041280-b01d-11eb-8e19-72a47f5955d4.png)
![image](https://user-images.githubusercontent.com/6963296/117553123-a002a480-b01d-11eb-9fc7-2a9f98867212.png)
As we can see, we only show the models with the 32mm tag assigned to them, once that tag has been turned on

##### Exclusive Filters
If you would like to remove any models from your list, you can use the Tags to exlude in filter drop down, which is to the right of the Tags to require in filter drop down. The exclude filter acts much like the include filter, but instead of only showing models with a specific set of tags, it excludes all models with any of the selected tags.
![image](https://user-images.githubusercontent.com/6963296/117553230-4353b980-b01e-11eb-8f07-e255032ab393.png)
As we can see, with the 32mm tag turned on, in the exlusion filter the jersey-barrier is no longer in my list of available models

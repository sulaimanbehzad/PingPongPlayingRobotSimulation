# All in One Guide for Delta Robot Simulation in Unity

2021-09-05

# Abstract

This is a walkthrough guide of delta robot simulation in Unity. This was done as a part of the Ping Pong Playing Robot project.

The main aim of this project was to set up a simulation environment in Unity for testing RL techniques. This environment consists of Delta robot, table, racket and ball.

# Installation

Unity Hub is a management tool/application used for managing all Unity projects and Unity installations.

![](RackMultipart20210905-4-1xlkygm_html_1ad40da16a0ae16f.png)

In the projects tab you can create a new project and access/manage the previous ones.

The Learn tab allows you to navigate through various sources/tutorials available for Unity.

To install unity, in Installs tab you can select the desired version and add it. This is the best and recommended approach to install Unity.

The Unity version used in this project is 2020.3.11f1 LTS (Long Term Support).

# Importing robot in Unity

There are two main approaches for importing a robot in Unity.

1.
## Importing the robot using URDF format and URDF importer plugin in Unity

This is the recommended approach if a URDF (Unified Robotics Definition Format) is available. Most of the complexities and challenges that was encountered in the second approach could be avoided if this method is followed.

The complete tutorial is available at:

[https://github.com/Unity-Technologies/Unity-Robotics-Hub/blob/main/tutorials/urdf\_importer/urdf\_tutorial.md](https://github.com/Unity-Technologies/Unity-Robotics-Hub/blob/main/tutorials/urdf_importer/urdf_tutorial.md)

1.
## Importing the robot using OBJ/FBX/Blend formats

The .fbx 3d files were obtained from .stl files. The challenges of this method and the solutions for them were as follows:

1. Scaling: the scaling factor of an object should be set to 0.01 from inspector menu.
2. Defining joints and setting parameters: this will be discussed in details in later sections.

# Introduction to Unity

The two articles provided below can give sufficient familiarity with Unity environment:

[Introduction to Unity: Getting Started – Part 1/2](https://www.raywenderlich.com/7514-introduction-to-unity-getting-started-part-1-2)

[Introduction to Unity: Getting Started – Part 2/2](https://www.raywenderlich.com/9175-introduction-to-unity-getting-started-part-2-2)

However, the main components and functionalities are explained below for a quick review:

GameObject: GameObjects are the fundamental objects in Unity that represent characters, props and scenery

![](RackMultipart20210905-4-1xlkygm_html_bd969adc255308d7.png)

Assets: The Assets folder is the main folder that contains the Assets used by a Unity project.

![](RackMultipart20210905-4-1xlkygm_html_81167089f7c3e3f0.png)

Scene: Run-time data structure for \*.unity file.

# Unity Package Manager

The Unity Package Manager is used to view which packages are available for installation or already installed in your project. It can be accessed from Window -\&gt; Package Manager.

The packages used in this project are:

ML Agents for RL

ROS TCP connector

# IDE Recommendation for working with Scripts

I strongly recommend JetBrains Rider Editor since it provides a unique set of tools in interaction with Unity.

# Adding Joints and setting parameters

We have multiple types of joints in Unity 3d, the ones used in delta robot are:

1. [Hinge joint](https://docs.unity3d.com/Manual/class-HingeJoint.html): Used for the connections between servo links and servo motors

![](RackMultipart20210905-4-1xlkygm_html_7d1ffec9a07f4523.png)

1. [Character joint](https://docs.unity3d.com/Manual/class-CharacterJoint.html): Used for connections between servo links and parallel links

![](RackMultipart20210905-4-1xlkygm_html_385f3ffa673d0500.png)

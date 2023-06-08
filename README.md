# ProductionFloorVisualization

This project was created for the purpose of my engineering thesis and to display the abilities of Mixed Reality in a professional design environment, in this case designing the layout of machines on a real-life production floor.

## Table of contents
* [Description](#description)
* [Use case](#use-case)
* [In real life tests](#in-real-life-tests)
* [Architecture](#the-client-app-architecture)

## Description

The project focused on creating a visualization that allows virtual objects 
to be placed in the real environment, displaying information about them, and modifying 
their position. Technological solutions were explored for developing augmented reality 
applications for both goggles and mobile devices, using off-the-shelf platforms
to enable interactions with mixed reality. The goal was to ensure a smooth process 
for designing the production floor.

## Use case

<img width="916" alt="Screenshot 2023-06-08 at 12 47 36" src="https://github.com/DeNatur/ProductionFloorVisualization/assets/28963517/9073e24d-214d-4096-bac4-0624897274c3">

## In real-life tests

The tests were conducted on the production floor in building B5 at Akademia Górniczo-Hutnicza w Krakowie. They involved showcasing the integration of real machines and holograms using advanced augmented reality technology. The purpose was to demonstrate how devices could be placed within the actual production floor and their positions modified accordingly. These tests successfully showed the ability to visualize and interact with virtual objects in a real environment.

Furthermore, the system allowed for the saving of object positions in the Azure cloud. This meant that the modified positions of devices could be securely stored and retrieved in subsequent application instances. The use of Azure Cloud ensured seamless access to the saved positions, enabling users to continue their design work from any device or location.

#### Recovering the saved position in the new app instance

https://github.com/DeNatur/ProductionFloorVisualization/assets/28963517/0a3c6277-056b-40e3-90b1-8bc28f17323e

### Modifying the position of a machine

https://github.com/DeNatur/ProductionFloorVisualization/assets/28963517/fd61b687-f1c9-4635-9404-0963229552c3

## Technologies

The underlying environment in which the application was developed is the Unity engine version 2020.3.26f1, which allows MRTK (Mixed Reality Toolkit) to work on the OpenXR platform and the object-oriented programming language C#. 
In addition, the following were used:
* Zenject, a dependency injection platform that allows for the inversion of dependencies by providing interfaces binding with their implementation and establishing the lifecycle of objects created by the Zenject.
* Unity Test Framework, a platform for creating unit and integration tests.
* NSubstitute library for creating mock objects used in unit tests.
* Azure Cloud to save the cloud anchors of the placed machines.

The application was built on the Universal Windows Platform, allowing it to run on the Hololens 2 device.

## The client app architecture

The implemented architecture (shown in a diagram below) is based on a simple clean architecture. All logic is separated from the view, and each successive layer of the application has a concrete direction of dependencies, so you can isolate and swap each successive layer and its implementation.

![Screenshot 2023-06-13 at 23 04 35](https://github.com/DeNatur/ProductionFloorVisualization/assets/28963517/cb405bb8-8736-4e5b-bf04-5a4953995703)


# FuzzPhyte Unity Tools

## Installer

This is a package that helps manage other FuzzPhyte Libraries.

## Setup & Design

This package assumes you're attempting to install other FuzzPhyte libraries and/or want to confirm manual dependency checks - see the FuzzPhyte Menu in the Unity Editor Toolbar for operation.

* FuzzPhyte-->Installer-->Dependency Installer
![FP Installer Editor Window](./Images/FPInstaller_0.png)
* If you want to install any existing package without using the package manager, you can put the entire GitHub URL here, for example https://github.com/jshull/FP_Utility.git, will fetch the repository and attempt to install it.
* If you want to install dependencies that are externally hosted and/or you see errors in the console, you need to know the package name and you will just put that assembly information in, for example, "com.fuzzphyte.utility"
  * The underlying FuzzPhyte package has to be using the dependency documentation system and should be updated beyond version 0.2.0

## Dependencies

This relies on Unity.Mathematics and the Unity Editor. Please see the [package.json](./package.json) file for more information and other information if installation fails.

## License Notes

* This software running a dual license
* Most of the work this repository holds is driven by the development process from the team over at Unity3D :heart: to their never ending work on providing fantastic documentation and tutorials that have allowed this to be born into the world.
* I personally feel that software and it's practices should be out in the public domain as often as possible, I also strongly feel that the capitalization of people's free contribution shouldn't be taken advantage of.
  * If you want to use this software to generate a profit for you/business I feel that you should equally 'pay up' and in that theory I support strong copyleft licenses.
  * If you feel that you cannot adhere to the GPLv3 as a business/profit please reach out to me directly as I am willing to listen to your needs and there are other options in how licenses can be drafted for specific use cases, be warned: you probably won't like them :rocket:

### Educational and Research Use MIT Creative Commons

* If you are using this at a Non-Profit and/or are you yourself an educator and want to use this for your classes and for all student use please adhere to the MIT Creative Commons License
* If you are using this back at a research institution for personal research and/or funded research please adhere to the MIT Creative Commons License
  * If the funding line is affiliated with an [SBIR](https://www.sbir.gov) be aware that when/if you transfer this work to a small business that work will have to be moved under the secondary license as mentioned below.

### Commercial and Business Use GPLv3 License

* For commercial/business use please adhere by the GPLv3 License
* Even if you are giving the product away and there is no financial exchange you still must adhere to the GPLv3 License

## Contact

* [John Shull](mailto:JShull@fuzzphyte.com)

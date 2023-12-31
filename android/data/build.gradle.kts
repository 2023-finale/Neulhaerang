plugins {
    id("com.android.library")
    id("org.jetbrains.kotlin.android")
}

android {
    namespace = "com.finale.neulhaerang.data"
    compileSdkVersion(rootProject.extra["compileSDKVersion"] as Int)

    defaultConfig {

        minSdkVersion(rootProject.extra["minimumSDKVersion"] as Int)
        testInstrumentationRunner = "androidx.test.runner.AndroidJUnitRunner"
        consumerProguardFiles("consumer-rules.pro")
    }

    buildTypes {
        release {
            isMinifyEnabled = false
            proguardFiles(
                getDefaultProguardFile("proguard-android-optimize.txt"),
                "proguard-rules.pro"
            )
        }
    }
    compileOptions {
        sourceCompatibility = JavaVersion.VERSION_11
        targetCompatibility = JavaVersion.VERSION_11
    }
    kotlinOptions {
        jvmTarget = "11"
    }
}

dependencies {
    // module
    implementation(project(":common"))
    // android
    implementation("androidx.core:core-ktx:${rootProject.extra["coreKtxVersion"]}")
    implementation("androidx.lifecycle:lifecycle-runtime-ktx:${rootProject.extra["lifecycleVersion"]}")
    // retrofit2
    implementation("com.squareup.retrofit2:retrofit:${rootProject.extra["retrofit2Version"]}")
    implementation("com.squareup.retrofit2:converter-gson:${rootProject.extra["retrofit2Version"]}")
    implementation("com.squareup.retrofit2:converter-scalars:${rootProject.extra["retrofit2Version"]}")
    implementation("com.github.ihsanbal:LoggingInterceptor:3.1.0") {
        exclude(group = "org.json", module = "json")
    }
    // test
    testImplementation("junit:junit:4.13.2")
    androidTestImplementation("androidx.test.ext:junit:1.1.5")
    androidTestImplementation("androidx.test.espresso:espresso-core:3.5.1")

    // datastore
    implementation("androidx.datastore:datastore-preferences:1.0.0")

    // unity
    implementation(project(":unityLibrary",))
    implementation(fileTree(mapOf("dir" to "..\\..\\UnityProject\\androidBuild\\unityLibrary\\libs", "include" to listOf("*.jar"))))
//    implementation fileTree(dir: 'libs', include: ['*.jar'])


}
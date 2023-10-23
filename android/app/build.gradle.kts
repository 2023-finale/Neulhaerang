plugins {
    id("com.android.application")
    id("org.jetbrains.kotlin.android")
}

android {
    namespace = "com.finale.neulhaerang"
    compileSdkVersion(rootProject.extra["compileSDKVersion"] as Int)

    defaultConfig {
        applicationId = "com.finale.neulhaerang"
        minSdk = 24
        targetSdk = rootProject.extra["targetSDKVersion"] as Int
        versionCode = 1
        versionName = "1.0"

        testInstrumentationRunner = "androidx.test.runner.AndroidJUnitRunner"
        vectorDrawables {
            useSupportLibrary = true
        }
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
        sourceCompatibility = JavaVersion.VERSION_1_8
        targetCompatibility = JavaVersion.VERSION_1_8
    }
    kotlinOptions {
        jvmTarget = "1.8"
    }
    buildFeatures {
        compose = true
    }
    composeOptions {
        kotlinCompilerExtensionVersion = "1.4.3"
    }
    packaging {
        resources {
            excludes += "/META-INF/{AL2.0,LGPL2.1}"
        }
    }
}

dependencies {

    // modules
    implementation(project(":common"))
    implementation(project(":data"))
    implementation(project(":domain"))
    implementation(project(":ui"))
    // android
    implementation("androidx.core:core-ktx:${rootProject.extra["coreKtxVersion"]}")
    implementation("androidx.lifecycle:lifecycle-runtime-ktx:2.6.2")
    implementation("androidx.activity:activity-compose:${rootProject.extra["composeUiVersion"]}")
    implementation(platform("androidx.compose:compose-bom:${rootProject.extra["composeBomVersion"]}"))
    implementation("androidx.compose.ui:ui")
    implementation("androidx.compose.ui:ui-graphics")
    implementation("androidx.compose.ui:ui-tooling-preview")
    implementation("androidx.compose.material3:material3")
    // navigation
    implementation("androidx.navigation:navigation-compose:${rootProject.extra["composeNavVersion"]}")
    // test
    testImplementation("junit:junit:4.13.2")
    androidTestImplementation("androidx.test.ext:junit:1.1.5")
    androidTestImplementation("androidx.test.espresso:espresso-core:3.5.1")
    androidTestImplementation(platform("androidx.compose:compose-bom:${rootProject.extra["composeBomVersion"]}"))
    androidTestImplementation("androidx.compose.ui:ui-test-junit4")
    // debug
    debugImplementation("androidx.compose.ui:ui-tooling")
    debugImplementation("androidx.compose.ui:ui-test-manifest")
}
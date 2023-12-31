package com.finale.neulhaerang.ui.app.main

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Add
import androidx.compose.material3.AlertDialog
import androidx.compose.material3.Button
import androidx.compose.material3.FloatingActionButton
import androidx.compose.material3.Icon
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Tab
import androidx.compose.material3.TabRow
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableIntStateOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.navigation.NavHostController
import androidx.navigation.compose.rememberNavController
import com.finale.neulhaerang.common.message.BlockMessage
import com.finale.neulhaerang.common.navigation.AppNavItem
import com.finale.neulhaerang.domain.MainScreenViewModel
import com.finale.neulhaerang.ui.R
import com.finale.neulhaerang.ui.app.navigation.NHLNavigationBar
import com.finale.neulhaerang.ui.app.navigation.stackNavigate
import com.finale.neulhaerang.ui.theme.Typography

@Composable
fun MainScreen(navController: NavHostController) {
    val viewModel = viewModel<MainScreenViewModel>(MainScreenViewModel.storeOwner)

    var alert by remember { mutableStateOf(false) }
    var message by remember { mutableStateOf("") }
    var tabIndex by remember { mutableIntStateOf(0) }
    val tabs = listOf("체크리스트", "우편함")


    val handleFAB = {
        if (viewModel.canIndolence)
            navController.stackNavigate(AppNavItem.CheckListCreation.route)
        else {
            message = BlockMessage.IndolenceBlock.message
            alert = true
        }
    }

    var beforeRoute by remember { mutableStateOf("") }
    val currentRoute = navController.currentDestination?.route ?: ""
    // 현재 라우트와 이전 라우트를 비교하여 다른 라우트에서 메인으로 온 경우 갱신
    if (beforeRoute != currentRoute) {
        beforeRoute = currentRoute
        if (currentRoute == AppNavItem.Main.route) {
            viewModel.backToMainScreen()
        }
    }

    Scaffold(
        bottomBar = { NHLNavigationBar(navController = navController) },
        floatingActionButton = {
            ChecklistCreationButton(handleFAB = handleFAB)
        }
    ) { innerPadding ->
        Column(
            modifier = Modifier.padding(innerPadding)
        ) {
            AppHeader(navController = navController)
            StatusBar(
                indolence = viewModel.indolence,
                tiredness = viewModel.tiredness
            )
            Calendar(
//                currentDate = currentDate,
                selectedDate = viewModel.selectedDate,
                setDateTime = viewModel::setDateTime
            )
            Column(modifier = Modifier.fillMaxWidth()) {
                TabRow(selectedTabIndex = tabIndex) {
                    tabs.forEachIndexed { index, title ->
                        Tab(text = { Text(title) },
                            selected = tabIndex == index,
                            onClick = { tabIndex = index })
                    }
                }
                when (tabIndex) {
                    0 -> CheckList(
                        navController,
                        viewModel.loading,
                        viewModel.routineList,
                        viewModel.todoList,
                        viewModel.selectedDate,
                        viewModel::checkCheckList
                    )

                    1 -> Letter(
                        loading = viewModel.loading,
                        letterText = viewModel.letterText,
                    )
                }
            }
        }
    }

    if (alert) {
        AlertDialog(
            onDismissRequest = { alert = false },
            confirmButton = {
                Button(onClick = { alert = false }) {
                    Text(text = stringResource(id = R.string.ok))
                }
            },
            text = {
                Text(text = message, style = MaterialTheme.typography.bodyLarge)
            }
        )
    }
}

@Composable
fun ChecklistCreationButton(handleFAB: () -> Unit) {
    FloatingActionButton(
        onClick = handleFAB
    ) {
        Icon(
            imageVector = Icons.Filled.Add,
            contentDescription = AppNavItem.CheckListCreation.description,
            modifier = Modifier
        )
    }
}

@Composable
fun AppHeader(
    modifier: Modifier = Modifier,
    navController: NavHostController,
) {
    Row(
        modifier = modifier
            .padding(16.dp)
            .fillMaxWidth(),
        horizontalArrangement = Arrangement.SpaceBetween,
        verticalAlignment = Alignment.CenterVertically
    ) {
        Text(text = "늘해랑", style = Typography.titleLarge)
        TextButton(onClick = { navController.stackNavigate(AppNavItem.Setting.route) }) {
            Text(
                text = "설정",
                color = MaterialTheme.colorScheme.onBackground,
                style = Typography.labelLarge
            )
        }
    }

}

@Preview
@Composable
fun Preview() {
    MainScreen(navController = rememberNavController())
}

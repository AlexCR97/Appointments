import fs from "fs";
import path from "path";

function main(options?: {
    packageJson?: {
        scripts?: {
            remove?: "all" | string[];
            keep?: string[];
        };
    };
}) {
    const projectPath = __dirname;

    let packageJson = getPackageJson();

    const packageJsonScriptsFieldsToRemove = (() => {
        if (options?.packageJson?.scripts === undefined) {
            return [];
        }

        if (options.packageJson.scripts.remove !== undefined) {
            if (options.packageJson.scripts.remove === "all") {
                return Object.keys(packageJson.scripts);
            } else {
                return Object.keys(packageJson.scripts).filter((key) =>
                    options.packageJson!.scripts!.remove!.includes(key)
                );
            }
        }

        if (options.packageJson.scripts.keep !== undefined) {
            return Object.keys(packageJson.scripts).filter(
                (key) => !options.packageJson!.scripts!.keep!.includes(key)
            );
        }

        return [];
    })();

    packageJson.scripts = removeFields(packageJson.scripts, packageJsonScriptsFieldsToRemove);

    packageJson = removeFields(packageJson, ["devDependencies"]);

    createPackageJson(packageJson);

    copyFile(path.join(projectPath, "..", ".npmignore"), path.join(projectPath, ".npmignore"));

    copyFile(path.join(projectPath, "..", "readme.md"), path.join(projectPath, "readme.md"));

    deleteFile(path.join(projectPath, "dev-imports.d.ts"));
    deleteFile(path.join(projectPath, "dev-imports.js"));
    deleteFile(path.join(projectPath, "dev-imports.js.map"));

    deleteFile(path.join(projectPath, "package.d.ts"));
    deleteFile(path.join(projectPath, "package.js.map"));

    function getPackageJson(): any {
        const packageJsonPath = path.join(projectPath, "..", "package.json");
        const packageJsonContent = fs.readFileSync(packageJsonPath).toString("utf-8");
        return JSON.parse(packageJsonContent);
    }

    function removeFields(source: any, fields: string[]): any {
        const result = JSON.parse(JSON.stringify(source));

        for (const field of fields) {
            if (field in result) {
                delete result[field];
            }
        }

        return result;
    }

    function createPackageJson(packageJson: any) {
        const packageJsonPath = path.join(projectPath, "package.json");
        const packageJsonContent = Buffer.from(JSON.stringify(packageJson, null, 2), "utf-8");
        fs.writeFileSync(packageJsonPath, packageJsonContent);
    }

    function copyFile(src: fs.PathLike, dest: fs.PathLike) {
        if (fs.existsSync(src)) {
            fs.copyFileSync(src, dest);
        }
    }

    function deleteFile(path: fs.PathLike) {
        if (fs.existsSync(path)) {
            fs.rmSync(path, { recursive: true });
        }
    }
}

main({
    packageJson: {
        scripts: {
            keep: ["preinstall", "postinstall"],
        },
    },
});
